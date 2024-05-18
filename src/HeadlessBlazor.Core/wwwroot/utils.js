/**
 * Gets the dimensions of the window.
 */
export function getWindowDimensions() {
    return new WindowDimensions();
}

/**
 * Gets the bounding client rectangle of the provided element.
 * @param {Element} element
 * @returns {ClientRect}
 */
export function getBoundingClientRect(element) {
    return new ClientRect(element);
}

export class WindowDimensions {
    constructor() {
        const html = document.documentElement;
        this.Height = html.clientHeight;
        this.Width = html.clientWidth;
    }
}

export class ClientRect {

    /**
     * @param {Element} element
     */
    constructor(element) {
        const rect = element.getBoundingClientRect();
        this.Top = rect.top;
        this.Left = rect.left;
        this.Bottom = rect.bottom;
        this.Right = rect.right;
    }
}