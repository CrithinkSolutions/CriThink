import * as types from './types';
import debounceAction from '../lib/debounceAction';
import axios from 'axios';
import { apiRequest, apiResponse } from './api';
import { newActionId } from '../lib/utils';

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

const loadAllSources = debounceAction(toDebounceLoadAllSources, 1000, { leading: true, trailing: false });

export {
    changeCurrentList,
    loadAllSources,
};