import {
    CHANGE_CURRENT_LIST,
    ALL_NEWS_SOURCES_RECEIVED,
    WHITELIST_SITE_ADDED,
    WHITELIST_SITE_REMOVED,
    BLACKLIST_SITE_ADDED,
    BLACKLIST_SITE_REMOVED,
} from '../actions/types';

const initialBackofficeState = {
    currentList: 'blacklist',
    blacklist: [],
    whitelist: [],
};

const backoffice = (state = initialBackofficeState, action) => {
    switch (action.type) {
        case CHANGE_CURRENT_LIST: 
            return {
                ...state,
                currentList: action.selectedList,
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
        case BLACKLIST_SITE_ADDED: {
            // Prevent shallow copy
            const blacklist = Object.assign([], state.blacklist);
            const { domain, classification, notes } = action.data;
            blacklist.push({
                uri: domain,
                classification,
                notes,
            });
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                blacklist,
            });
        };
        case WHITELIST_SITE_ADDED: {
            // Prevent shallow copy
            const whitelist = Object.assign([], state.whitelist);
            const { domain, classification, notes } = action.data;
            whitelist.push({
                uri: domain,
                classification,
                notes,
            });
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                whitelist,
            });
        };
        case BLACKLIST_SITE_REMOVED: {
            // Prevent shallow copy
            const blacklist = Object.assign([], state.blacklist);
            
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                blacklist: blacklist.filter(x => x.uri !== action.data.site),
            })
        };
        case WHITELIST_SITE_REMOVED: {
            // Prevent shallow copy
            const whitelist = Object.assign([], state.whitelist);
            
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                whitelist: whitelist.filter(x => x.uri !== action.data.site),
            })
        };
        default:
            return state;
    };
};

export default backoffice;