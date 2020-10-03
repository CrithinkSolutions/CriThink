import * as types from './types';
import debounceAction from '../lib/debounceAction';
import axios from 'axios';
import { newActionId } from '../lib/utils';
import { apiRequest, apiResponse } from './api';

function getBaseUri() {
    if (process.env.NODE_ENV === 'production') {
        axios.defaults.baseURL = 'https://crithinkdemo.com'
    }
    else {
        axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*'
        axios.defaults.baseURL = 'https://localhost:5001'
    }
}

getBaseUri();

function questionReducer (questions) {
    return {
        type: types.QUESTIONS,
        questions,
    };
}

function newsReducer (news) {
    return {
        type: types.GET_NEWS,
        news,
    };
}

function demonewsReducer (dnews) {
    return {
        type: types.GET_DEMO_NEWS,
        dnews,
    };
}

function toDebounceGetQuestions () {
    return (dispatch) => {
        const actionId = newActionId('Getting question for H.E.A.D.', 'getQuestions');
        dispatch(apiRequest(actionId));
        axios.get('/api/news-analyzer/question')
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiResponse(actionId));
                    dispatch(questionReducer(res.data.result));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
    };
}

const getQuestions = debounceAction(toDebounceGetQuestions, 1000, { leading: true, trailing: false });

function toDebounceGetNews (uri) {
    return (dispatch) => {
        const actionId = newActionId('Get info form the news', 'getNews');
        dispatch(apiRequest(actionId));
        axios.post('/api/news-analyzer/scrape-news', {
            uri:uri,
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiResponse(actionId));
                    dispatch(newsReducer(res.data.result));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
    };
}

const getNews = debounceAction(uri => toDebounceGetNews(uri), 1000, { leading: true, trailing: false });

function toDebounceGetDemoNews () {
    return (dispatch) => {
        const actionId = newActionId('Getting demo news', 'getDemoNews');
        dispatch(apiRequest(actionId));
        axios.get('/api/demo/demo-news')
            .then(res => {
                if(res.status === 200) {
                    dispatch(demonewsReducer(res.data.result));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            });
    };
}

const getDemoNews = debounceAction(toDebounceGetDemoNews, 1000, { leading: true, trailing: false });

function selectNews ({uri, title}) {
    return {
        type: types.NEWS_SELECTED,
        uri,
        title,
    };
}

function newsClassificationReceived ({classification, description}) {
    return {
        type: types.NEWS_CLASSIFICATION_RECEIVED,
        classification,
        description,
    };
}

const getNewsClassification = debounceAction(toDebounceGetNewsClassification, 1000, { leading: true, trailing: false });

function toDebounceGetNewsClassification (uri) {
    return (dispatch) => {
        const actionId = newActionId('Getting Classification of the current news', 'getNewsClassification');
        dispatch(apiRequest(actionId));
        axios.get(`/api/news-source?uri=${ encodeURIComponent(uri) }`)
            .then(res => {
                if(res.status === 200) {
                    dispatch(newsClassificationReceived(res.data));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                if (err.response.status === 404) {
                    dispatch(newsClassificationReceived({
                        classification: 'Unkown',
                        description: 'We don\'t have any information',
                    }));
                }
                dispatch(apiResponse(actionId));
            });
    };
}

export {
    getQuestions,
    getNews,
    getDemoNews,
    selectNews,
    getNewsClassification,
};