import {
    CHANGE_CURRENT_LIST,
    ALL_NEWS_SOURCES_RECEIVED,
    ADD_WHITELIST_SITE,
    REMOVE_WHITELIST_SITE,
    ADD_BLACKLIST_SITE,
    REMOVE_BLACKLIST_SITE,
} from '../actions/types';

const initialBackofficeState = {
    currentList: 'blacklist',
    blacklist: [],
    whitelist: [],
};

const backoffice = (state = initialBackofficeState, action) => {
    switch (action.type) {
        case CHANGE_CURRENT_LIST: {
            return {
                ...state,
                currentList: action.selectedList,
            };
        };
        case ALL_NEWS_SOURCES_RECEIVED: {
            const whitelist = action.data.filter(x => x.classification === 'Trusted' || x.classification === 'Satiric');
            const blacklist = action.data.filter(x => x.classification === 'Cospiracy' || x.classification === 'Fake');
            return {
                ...state,
                blacklist,
                whitelist,
            };
        };
        default:
            return state;
    };
};

export default backoffice;