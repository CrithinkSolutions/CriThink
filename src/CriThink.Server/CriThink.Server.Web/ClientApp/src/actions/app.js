import * as types from './types';

export function openDialog(title, message, confirmAction) {
    return {
        type: types.OPEN_DIALOG,
        title,
        message,
        confirmAction,
    };
}

export function openCustomDialog(dialog) {
    return {
        type: types.OPEN_CUSTOM_DIALOG,
        dialog,
    };
}

export function closeDialog() {
    return {
        type: types.CLOSE_DIALOG,
    };
}