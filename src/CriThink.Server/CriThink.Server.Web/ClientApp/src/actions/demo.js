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

function toDebounceGetNews() {
    return (dispatch) => {
        const actionId = newActionId('Getting question for H.E.A.D.', 'getQuestions');
        dispatch(apiRequest(actionId));
        axios.post('/api/news-analyzer/scrape-news', {
            uri:'https://www.ilsole24ore.com/art/coronavirus-ultime-notizie-oggi-italia-1434-contagi-95990-tamponi-14-vittime-ADgLdFo'
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

const getNews = debounceAction(toDebounceGetNews, 1000, { leading: true, trailing: false });

export {
	getQuestions,
    getNews
};