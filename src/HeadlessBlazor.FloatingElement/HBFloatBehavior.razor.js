import { autoUpdate, computePosition, flip } from './floating-ui.dom.browser.min.mjs';

export class HBFloatBehavior {

    /**
     * @static
     * @param {Element} anchor
     * @param {Element} content
     * @param {any} options
     * @returns {HBFloatBehavior}
     */
    static createInstance(anchor, content, options) {
        return new HBFloatBehavior(anchor, content, options);
    }

    /**
     * Creates a new instance of HBFloatBehavior.
     * @constructor
     * @param {Element} anchor
     * @param {Element} content
     * @param {any} options
     */
    constructor(anchor, content, options) {
        this.anchor = anchor;
        this.content = content;
        this.options = options;
        this.dispose = autoUpdate(anchor, content, this.update);
    }

    updateOptions = (options) => {
        this.options = options;
        this.update();
    }

    update = () => {
        const side = this.options.side || 'bottom';
        const align = this.options.alignment || 'start';

        computePosition(this.anchor, this.content, {
            placement: `${side}-${align}`.toLowerCase(),
            middleware: [flip()]
        }).then(({ x, y }) => {
            Object.assign(this.content.style, {
                left: `${x}px`,
                top: `${y}px`,
            });
        })
    }
}