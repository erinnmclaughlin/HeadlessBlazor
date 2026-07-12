export class HBToastHostItem {

    /**
     * Resolves after the browser has had a chance to paint the current frame, so a style
     * change applied before the call and one applied after are separated by a paint. Uses a
     * double requestAnimationFrame: the first callback runs before the next paint, the second
     * after it, guaranteeing the initial state has been rendered.
     * @returns {Promise<void>}
     */
    static nextFrame() {
        return new Promise(resolve => {
            requestAnimationFrame(() => requestAnimationFrame(() => resolve()));
        });
    }
}
