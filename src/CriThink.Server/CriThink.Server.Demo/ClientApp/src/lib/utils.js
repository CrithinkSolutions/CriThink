import { uniqueId } from 'lodash';

const newActionId = (message, label) => ({ id: uniqueId(), message, label });

const validHostname = (hostname) => {
    const regEx = RegExp(/^([a-z0-9][a-z0-9-]*\.)+[a-z0-9][a-z0-9-]*$/);

    return regEx.test(hostname);
};

export {
    newActionId,
    validHostname,
};