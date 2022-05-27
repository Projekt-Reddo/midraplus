import { CONNECT_SIGNALR, DISCONNECT_SIGNALR } from "store/actions";

const connection = {};

const connectionReducer = (state = connection, action: ActionType) => {
    switch (action.type) {
        case CONNECT_SIGNALR:
            return action.payload;

        case DISCONNECT_SIGNALR:
            return {};

        default:
            return state;
    }
};

export default connectionReducer;
