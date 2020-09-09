import { uniqueId } from 'lodash';

const newActionId = (message, label) => ({ id: uniqueId(), message, label });

const validHostname = (hostname) => {
    const regEx = RegExp('^([a-z0-9][a-z0-9-]*.)+[a-z0-9][a-z0-9-]*$', 'g');

    return regEx.test();
};

export {
    newActionId,
    validHostname,
};