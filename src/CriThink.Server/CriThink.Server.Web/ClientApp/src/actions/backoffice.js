import * as types from './types';
import debounceAction from '../lib/debounceAction';
import axios from 'axios';
import { apiRequest, apiResponse } from './api';
import { newActionId } from '../lib/utils';
import { closeDialog } from './app';

function changeCurrentList(selectedList) {
    return {
        type: types.CHANGE_CURRENT_LIST,
        selectedList,
    }
}

function newsReceived(data) {
    return {
        type: types.ALL_NEWS_SOURCES_RECEIVED,
        data,
    };
}

function toDebounceLoadAllSources() {
    return (dispatch) => {
        const actionId = newActionId('Loading news sources...', 'getAllSources');
        dispatch(apiRequest(actionId));
        axios.get('/api/news-source/all')
            .then(res => {
                if (res.status === 200) {
                    dispatch(newsReceived(res.data));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
        return;
    };
}

function blacklistNewsAdded(data) {
    return {
        type: types.BLACKLIST_SITE_ADDED,
        data,
    };
}

function whitelistNewsAdded(data) {
    return {
        type: types.WHITELIST_SITE_ADDED,
        data,
    };
}

const loadAllSources = debounceAction(toDebounceLoadAllSources, 1000, { leading: true, trailing: false });

function toDebounceAddNewsSource({domain, classification, notes, list}) {
    return (dispatch) => {
        const actionId = newActionId('Adding new news source...', 'addNewsSource');
        dispatch(apiRequest(actionId));
        axios.post('/api/news-source', {
            uri: `https://${domain}`,
            classification,
            notes,
        })
            .then(res => {
                if(res.status === 204) {
                    if (list === 'blacklist') {
                        dispatch(blacklistNewsAdded({domain, classification, notes}));
                    }
                    else {
                        dispatch(whitelistNewsAdded({domain, classification, notes}));
                    }
                    dispatch(apiResponse(actionId));
                    dispatch(closeDialog());
                }
            })
            .then(err => {
                dispatch(apiResponse(actionId));
            })
    };
}

const addNewsSource = debounceAction(toDebounceAddNewsSource, 1000, { leading: true, trailing: false });

function blacklistNewsRemoved(site) {
    return {
        type: types.BLACKLIST_SITE_REMOVED,
        data: { site },
    };
}

function toDebounceRemoveBlacklistedSite(site) {
    return (dispatch) => {
        const actionId = newActionId(`Removing '${site}' from sources...`, 'confirmationDialog');
        dispatch(apiRequest(actionId));
        axios.request({
            method: 'DELETE',
            data: {
                uri: `https://${site}`,
            },
            url: '/api/news-source/blacklist'
        })
            .then(res => {
                if (res.status === 204) {
                    dispatch(blacklistNewsRemoved(site));
                }
                dispatch(apiResponse(actionId));
                dispatch(closeDialog());
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
    };
}

const removeBlacklistedSite = debounceAction(toDebounceRemoveBlacklistedSite, 1000, { leading: true, trailing: false });

function whitelistNewsRemoved(site) {
    return {
        type: types.WHITELIST_SITE_REMOVED,
        data: { site },
    };
}

function toDebounceRemoveWhitelistedSite(site) {
    return (dispatch) => {
        const actionId = newActionId(`Removing '${site}' from sources...`, 'confirmationDialog');
        dispatch(apiRequest(actionId));
        axios.request({
            method: 'DELETE',
            data: {
                uri: `https://${site}`,
            },
            url: '/api/news-source/whitelist'
        })
            .then(res => {
                if (res.status === 204) {
                    dispatch(whitelistNewsRemoved(site));
                }
                dispatch(apiResponse(actionId));
                dispatch(closeDialog());
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
    };
}

const removeWhitelistedSite = debounceAction(toDebounceRemoveWhitelistedSite, 1000, { leading: true, trailing: false });

export {
    changeCurrentList,
    loadAllSources,
    addNewsSource,
    removeBlacklistedSite,
    removeWhitelistedSite,
};