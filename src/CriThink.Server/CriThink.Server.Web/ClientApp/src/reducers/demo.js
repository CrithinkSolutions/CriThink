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
            const questions = action.questions.sort((a, b) => a.order - b.order);
            return {
                ...state,
                questionH: questions[0].content,
                questionE: questions[1].content,
                questionA: questions[2].content,
                questionD: questions[3].content,
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
            let color;
            switch(action.classification) {
                case 'Reliable':
                case 'Satirical':
                    color = 'green';
                    break;
                case 'Fake News':
                    color = 'red';
                    break;
                case 'Conspiracist':
                    color = 'orange';
                    break;
                default:
                    color = 'grey';
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
