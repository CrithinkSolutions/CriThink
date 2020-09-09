import {
	QUESTIONS,
	GET_NEWS
} from '../actions/types';

const initialAuthState = {
    questionH: '',
    questionE: '',
    questionA: '',
    questionD: '',
    newsHeader: '',
    newsBody: ''
};

const demoreducer = (state = initialAuthState, action) => {
	switch(action.type) {
		case QUESTIONS: 
			return {
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
		default:
			return state;
	}
}


export default demoreducer;
