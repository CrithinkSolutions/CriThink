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
        const actionId = newActionId('Get info form the news', 'getNews');
        dispatch(apiRequest(actionId));
        axios.post('/api/news-analyzer/scrape-news', {
            uri:'https://news.sky.com/story/boris-johnsons-move-to-override-parts-of-brexit-deal-has-eroded-trust-says-irish-pm-12067606'
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