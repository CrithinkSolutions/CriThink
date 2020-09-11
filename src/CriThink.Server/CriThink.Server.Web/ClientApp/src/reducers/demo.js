import {
	QUESTIONS,
	GET_NEWS,
	GET_DEMO_NEWS,
	GET_DEMO_NEWS_SELECT
} from '../actions/types';

const initialAuthState = {
    questionH: '',
    questionE: '',
    questionA: '',
    questionD: '',
    newsHeader: '',
    newsBody: '',
    demoNews: {},
    demoNewsSelected: {}
};

const demoreducer = (state = initialAuthState, action) => {
	switch(action.type) {
		case QUESTIONS: 
			return {
				...state,
				questionH: action.questions[2].content,
				questionE: action.questions[1].content,
				questionA: action.questions[0].content,
				questionD: action.questions[3].content
			};
		case GET_NEWS:
			return {
				...state,
				newsHeader: action.news.title,
				newsBody: action.news.body
			}
		case GET_DEMO_NEWS:
			return {
				...state,
				demoNews: action.dnews,
			}
		case GET_DEMO_NEWS_SELECT:
			return {
				...state,
				demoNewsSelected: {
					uri: action.uri,
					type: action.classification
				}
			}
		default:
			return state;
	}
}


export default demoreducer;
