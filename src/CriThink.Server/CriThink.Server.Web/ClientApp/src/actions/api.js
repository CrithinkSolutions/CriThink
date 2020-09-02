import * as types from './types';

export function apiRequest({ id, message, label }) {
    return {
        type: types.API_REQUEST,
        id,
        label,
        loadingMessage: message,
    };
}

export function setResponse(id, label, error, message, type) {
    return {
        type: types.API_RESPONSE,
        id,
        label,
        error,
        message,
        messageType: type,
    };
}

export function apiResponse({ id, label }) {
    return (dispatch) => {
        dispatch(setResponse(id, label));
    };
}