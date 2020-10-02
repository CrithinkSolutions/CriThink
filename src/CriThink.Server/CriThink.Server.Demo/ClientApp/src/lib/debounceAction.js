import _ from 'lodash';

export default function debounceAction (action, wait, options) {
    const debounced = _.debounce(
        (dispatch, actionArgs) => dispatch(action(...actionArgs)),
        wait,
        options
    );

    const thunk = (...actionArgs) => dispatch => debounced(dispatch, actionArgs);

    thunk.cancel = debounced.cancel;

    return thunk;
}
