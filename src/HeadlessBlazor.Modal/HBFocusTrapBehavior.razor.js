const FOCUSABLE_SELECTOR = [
    'a[href]',
    'area[href]',
    'button:not([disabled])',
    'input:not([disabled]):not([type="hidden"])',
    'select:not([disabled])',
    'textarea:not([disabled])',
    'iframe',
    '[tabindex]:not([tabindex="-1"])',
    '[contenteditable="true"]'
].join(',');

export class HBFocusTrapBehavior {

    /**
     * @param {Element} element
     * @returns {HBFocusTrapBehavior}
     */
    static createInstance(element) {
        return new HBFocusTrapBehavior(element);
    }

    /**
     * Traps Tab focus within the element, moves focus inside on creation, and
     * restores focus to the previously-focused element on disposal.
     * @constructor
     * @param {Element} element
     */
    constructor(element) {
        this.element = element;
        this.previouslyFocused = document.activeElement;

        element.addEventListener('keydown', this.handleKeyDown);

        const focusable = this.getFocusableElements();
        (focusable[0] ?? element).focus();
    }

    dispose() {
        this.element.removeEventListener('keydown', this.handleKeyDown);

        if (this.previouslyFocused instanceof HTMLElement) {
            this.previouslyFocused.focus();
        }
    }

    /**
     * @returns {HTMLElement[]}
     */
    getFocusableElements() {
        return Array
            .from(this.element.querySelectorAll(FOCUSABLE_SELECTOR))
            .filter(el => el.offsetParent !== null || el === document.activeElement);
    }

    /**
     * @param {KeyboardEvent} e
     */
    handleKeyDown = e => {
        if (e.key !== 'Tab') {
            return;
        }

        const focusable = this.getFocusableElements();

        if (focusable.length === 0) {
            e.preventDefault();
            this.element.focus();
            return;
        }

        const first = focusable[0];
        const last = focusable[focusable.length - 1];
        const active = document.activeElement;

        if (e.shiftKey && (active === first || active === this.element)) {
            e.preventDefault();
            last.focus();
        } else if (!e.shiftKey && active === last) {
            e.preventDefault();
            first.focus();
        }
    }
}
