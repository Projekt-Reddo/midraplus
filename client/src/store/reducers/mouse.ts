import { RECEIVE_MOUSE } from "store/actions";

const mouses: object[] = [];

const mouseReducer = (state = mouses, action: ActionType) => {
    switch (action.type) {
        case RECEIVE_MOUSE: {
            if (
                state.some((item: any) => item.userId === action.payload.userId)
            ) {
                return state.map((item: any) => {
                    if (item.userId === action.payload.userId) {
                        return {
                            ...item,
                            userName: action.payload.userName,
                            x: action.payload.x,
                            y: action.payload.y,
                            isMove: action.payload.isMove,
                        };
                    }
                    return item;
                });
            }
            return [
                ...state,
                {
                    userId: action.payload.userId,
                    userName: "",
                    x: 0,
                    y: 0,
                    isMove: false,
                },
            ];
        }
        default:
            return state;
    }
};

export default mouseReducer;
