import { LOGIN, LOGOUT } from "store/actions";

const user = {
    user: null,
    isAuthenticated: false,
};

const userReducer = (state = user, action: ActionType) => {
    switch (action.type) {
        case LOGIN:
            return { user: action.payload, isAuthenticated: true };

        case LOGOUT:
            return { user: null, isAuthenticated: false };

        default:
            return state;
    }
};

export default userReducer;
