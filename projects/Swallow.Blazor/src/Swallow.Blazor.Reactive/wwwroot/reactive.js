import htmx from "./htmx.js";
import morphdom from "./morphdom.js";
import { getEventObject } from "./events.js";

function processIsland(island) {
    const element = document.querySelector(`[rx-island='${island}']`);
    if (!element) {
        return;
    }

    htmx.defineExtension("morphdom-swap", {
        isInlineSwap: swapStyle => swapStyle === "morphdom",
        handleSwap: (swapStyle, target, fragment) => {
            if (swapStyle === "morphdom") {
                const source = fragment.nodeType === Node.DOCUMENT_FRAGMENT_NODE
                    ? fragment.firstElementChild || fragment.firstChild
                    : fragment.outerHTML;

                morphdom(target, source, { onBeforeElUpdated: (fromEl, toEl) => !fromEl.isEqualNode(toEl) });
                return [target];
            }
        }
    });

    element.setAttribute("hx-trigger", "initialize once");
    element.setAttribute("hx-post", "/" + element.getAttribute("rx-route"));
    element.setAttribute("hx-target", "this");
    element.setAttribute("hx-swap", "morphdom");
    element.setAttribute("hx-sync", "this:replace");
    element.setAttribute("hx-indicator", "this");

    attachAttributes({ detail: { elt: element }});

    document.body.addEventListener("htmx:configRequest", configureRequest);
    document.body.addEventListener("htmx:afterSwap", attachAttributes);
    htmx.process(element);

    htmx.trigger(element, "initialize");
}


function configureRequest(evnt) {
    const container = evnt.detail?.elt.closest("[rx-island]");
    if (!container) {
        return;
    }

    for (const el of [...container.querySelectorAll("[rx-state]")]) {

        evnt.detail.parameters[el.name] = el.value;
    }

    evnt.detail.headers["rx-island"] = container.getAttribute("rx-island");
    evnt.detail.headers["rx-trigger"] = evnt.target.getAttribute("rx-id");

    if (evnt.detail.triggeringEvent) {
        evnt.detail.headers["rx-event"] = evnt.detail.triggeringEvent.type;

        const transformer = getEventObject(evnt.detail.triggeringEvent.type);
        if (transformer) {
            evnt.detail.parameters["__event"] = JSON.stringify(transformer(evnt.detail.triggeringEvent));
        }
    }
}

function attachAttributes(evnt) {
    const container = evnt.detail?.elt.closest("[rx-island]");
    if (!container) {
        return;
    }

    const route = "/" + container.getAttribute("rx-route");
    for (const el of [...evnt.detail.elt.querySelectorAll("[rx-id]")]) {
        el.setAttribute("hx-post", route);
    }
}

const reactive = { process: processIsland }
export default reactive;
