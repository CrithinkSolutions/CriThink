import * as types from './types';
import debounceAction from '../lib/debounceAction';
import axios from 'axios';
import { newActionId } from '../lib/utils';
import { apiRequest, apiResponse, apiError, apiSuccess } from './api';

function questionReducer(questions) {
	return {
        type: types.QUESTIONS,
        questions
    }
}

function newsReducer(news) {
    return {
        type: types.GET_NEWS,
        news
    }
}

function demonewsReducer(dnews) {
    return {
        type: types.GET_DEMO_NEWS,
        dnews
    }
}

function toDebounceGetQuestions() {
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
            })
    };
}

const getQuestions = debounceAction(toDebounceGetQuestions, 1000, { leading: true, trailing: false });

function toDebounceGetNews(uri) {
    return (dispatch) => {
        const actionId = newActionId('Get info form the news', 'getNews');
        dispatch(apiRequest(actionId));
        axios.post('/api/news-analyzer/scrape-news', {
            uri:uri
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiResponse(actionId));
                    dispatch(newsReducer(res.data.result));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            })
    };
}

const getNews = debounceAction(uri => toDebounceGetNews(uri), 1000, { leading: true, trailing: false });

function toDebounceGetDemoNews() {
    return (dispatch) => {
        const actionId = newActionId('Getting demo news', 'getDemoNews');
        dispatch(apiRequest(actionId));
        axios.get('/api/news-analyzer/demo-news')
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiResponse(actionId));
                    dispatch(demonewsReducer(res.data.result));
                }
            })
            .catch(err => {
                dispatch(apiResponse(actionId));
            })
    };
}

const getDemoNews = debounceAction(toDebounceGetDemoNews, 1000, { leading: true, trailing: false });

function getDemoNewsSelected(uri, classification) {
    return {
        type: types.GET_DEMO_NEWS_SELECT,
        uri: uri,
        classification: classification
    }
}


export {
	getQuestions,
    getNews,
    getDemoNews,
    getDemoNewsSelected
};