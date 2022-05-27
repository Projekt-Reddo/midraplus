import { INITLC } from "store/actions";

const lc = {};

const lcReducer = (state = lc, action: ActionType) => {
    switch (action.type) {
        case INITLC:
            return action.payload;
        default:
            return state;
    }
};

export default lcReducer;
