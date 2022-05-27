import { RECEIVE_MESSAGE } from "store/actions";

interface ChatItem {
    user: any;
    message: string;
    timestamp: Date;
}

const chat: ChatItem[] = [];

const chatReducer = (state = chat, action: ActionType) => {
    switch (action.type) {
        case RECEIVE_MESSAGE: {
            return [...state, action.payload];
        }

        default:
            return state;
    }
};

export default chatReducer;
