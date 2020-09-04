import * as types from './types';
import React from 'react';
import { Button, Form, Grid, Message, Icon, Divider } from 'semantic-ui-react'

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

export function apiError(err) {
    return {
        type: types.API_ERROR,
        render:
        <Message negative>
            <Icon name='warning' />
            <b>{err}</b>
        </Message>
    };
}

export function apiSuccess(success) {
    return {
        type: types.API_SUCCESS,
        render:
        <Message positive>
            <Icon name='check' />
            <b>{success}</b>
        </Message>
    }
}