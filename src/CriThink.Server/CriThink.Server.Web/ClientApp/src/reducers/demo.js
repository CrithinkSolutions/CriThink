import {
    QUESTIONS,
    GET_NEWS,
    GET_DEMO_NEWS,
    NEWS_SELECTED,
    NEWS_CLASSIFICATION_RECEIVED,
} from '../actions/types';

const initialAuthState = {
    questionH: '',
    questionE: '',
    questionA: '',
    questionD: '',
    newsHeader: '',
    newsBody: '',
    demoNews: [],
    demoNewsSelected: {},
};

const demoreducer = (state = initialAuthState, action) => {
    switch(action.type) {
        case QUESTIONS:
            return {
                ...state,
                questionH: action.questions[2].content,
                questionE: action.questions[1].content,
                questionA: action.questions[0].content,
                questionD: action.questions[3].content,
            };
        case GET_NEWS:
            return {
                ...state,
                newsHeader: action.news.title,
                newsBody: action.news.body,
            };
        case GET_DEMO_NEWS:
            return {
                ...state,
                demoNews: action.dnews,
            };
        case NEWS_SELECTED:
            return {
                ...state,
                demoNewsSelected: {
                    uri: action.uri,
                    title: action.title,
                },
            };
        case NEWS_CLASSIFICATION_RECEIVED:
            let color = 'grey';
            switch(action.classification) {
                case 'Trusted':
                case 'Satiric':
                    color = 'green';
                    break;
                case 'Fake':
                    color = 'red';
                    break;
                case 'Cospiracy':
                    color = 'orange';
                    break;
            }
            return {
                ...state,
                demoNewsSelected: {
                    ...state.demoNewsSelected,
                    classification: action.classification,
                    description: action.description,
                    color,
                },
            };
        default:
            return state;
    }
};


export default demoreducer;
