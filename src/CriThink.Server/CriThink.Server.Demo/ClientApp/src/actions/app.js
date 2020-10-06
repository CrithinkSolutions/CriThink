import * as types from './types';
import React from 'react';
import ConfirmationModal from '../components/modals/ConfirmationModal';

export function openDialog (title, message, confirmAction) {
    return {
        type: types.OPEN_DIALOG,
        title,
        message,
        confirmAction,
    };
}

export function openCustomDialog (dialog) {
    return {
        type: types.OPEN_CUSTOM_DIALOG,
        dialog,
    };
}

export function openConfirmationDialog (title, body, confirmAction, data) {
    return {
        type: types.OPEN_CONFIRMATION_DIALOG,
        dialog: <ConfirmationModal
            title={title}
            body={body}
            confirmationHandler={confirmAction}
            data={data}
        />,
    };
}

export function closeDialog () {
    return {
        type: types.CLOSE_DIALOG,
    };
}

export function enabledRoutesReceived (data) {
    return {
        type: types.ENABLED_ROUTES_RECEIVED,
        data,
    };
}