# Swallow.Charts

Charts for Blazor. Simple as that. But it has a twist: These charts do not use a `<canvas>` but render to SVG!

### But... why? Just wrap an existing library and call it a day

`<canvas>` can only be manipulated via JS on the client. Static server-side rendering should be just that: no client-side work needed. Which isn't
really possible when using a `<canvas>`. So instead, all of these charts render to an `<svg>`, which means they're a first-class citizen in the DOM.

Cool, right? Yeah, that's what I thought.

## Roadmap

- [x] Line chart
- [x] Pie chart
- [ ] Bar chart
- [ ] Theming / coloring
- [ ] Legend / labels
- [ ] Multiple lines
