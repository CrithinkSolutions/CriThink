import { 
    API_REQUEST,
    API_RESPONSE,
    OPEN_CUSTOM_DIALOG,
    OPEN_CONFIRMATION_DIALOG,
    CLOSE_DIALOG
} from '../actions/types';
import ConfirmationModal from '../components/modals/ConfirmationModal';

const initialAppState = {
    loading: [],
    loadingMessage: '',
    dialog: undefined,
    dialogOpen: false,
};

const app = (state = initialAppState, action) => {
    switch (action.type) {
        case API_REQUEST: {
            // Prevent shallow copy
            const loading = Object.assign([], state.loading);
            loading.push({
                id: action.id,
                label: action.label,
            });
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                loading,
                loadingMessage: action.loadingMessage,
            });
        };
        case API_RESPONSE: {
            // Prevent shallow copy
            const loading = Object.assign([], state.loading);
            const index = loading.findIndex(item => item.id === action.id && item.label === action.label);

            if (index > -1) {
                loading.splice(index, 1);
            }
            // Prevent shallow copy
            return Object.assign({}, {
                ...state,
                loading: [],
            });
        };
        case OPEN_CUSTOM_DIALOG: {
            return {
                ...state,
                dialogOpen: true,
                dialog: action.dialog,
            };
        };
        case OPEN_CONFIRMATION_DIALOG: {
            return {
                ...state,
                dialogOpen: true,
                dialog: action.dialog,
            }
        };
        case CLOSE_DIALOG: {
            return {
                ...state,
                dialogOpen: false,
                // dialog: undefined,
            };
        };
        default:
            return state;
    }
}

export default app;
