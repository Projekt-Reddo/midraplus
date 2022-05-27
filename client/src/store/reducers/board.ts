import { JOIN_ROOM, LEAVE_ROOM } from "store/actions";

const board: string = "";

const boardReducer = (state = board, action: ActionType) => {
    switch (action.type) {
        case JOIN_ROOM: {
            return action.payload.board;
        }

        case LEAVE_ROOM: {
            return "";
        }

        default:
            return state;
    }
};

export default boardReducer;
