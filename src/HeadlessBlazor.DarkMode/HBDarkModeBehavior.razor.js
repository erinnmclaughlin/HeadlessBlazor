export class HBDarkModeBehavior {

    /**
     * @param {any} dotNetRef
     * @returns {HBDarkModeBehavior}
     */
    static createInstance(dotNetRef) {
        return new HBDarkModeBehavior(dotNetRef);
    }

    /**
     * @constructor
     * @param {any} dotNetRef
     */
    constructor(dotNetRef) {
        this.dotNetRef = dotNetRef;
        this.mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        this.mediaQuery.addEventListener('change', this.handleChange);
    }

    /**
     * @returns {boolean}
     */
    isDarkMode() {
        return this.mediaQuery.matches;
    }

    dispose() {
        this.mediaQuery.removeEventListener('change', this.handleChange);
    }

    /**
     * @param {MediaQueryListEvent} e
     */
    handleChange = e => {
        this.dotNetRef.invokeMethodAsync('NotifyChangeAsync', e.matches);
    }
}
