import { ONLINE_USERS } from "store/actions";

const users: object[] = [];

const onlineUsersReducer = (state = users, action: ActionType) => {
    switch (action.type) {
        case ONLINE_USERS:
            return action.payload;
        default:
            return state;
    }
};

export default onlineUsersReducer;
