import { uniqueId } from "lodash"
import { cssNumber } from "jquery";

export const newActionId = (message, label) => ({ id: uniqueId(), message, label });