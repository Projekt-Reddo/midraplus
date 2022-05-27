import { RECEIVE_SHAPE, REMOVE_SHAPE, INIT_SHAPES, CLEAR_SHAPE_WHEN_LEAVE_ROOM, CLEAR_ALL } from "store/actions";

const shapes: object[] = [];

const shapeReducer = (state = shapes, action: ActionType) => {
    switch (action.type) {
        case INIT_SHAPES:
            return action.payload;
            
        case RECEIVE_SHAPE:
            return [...state, action.payload];

        case REMOVE_SHAPE: {
            return state.filter((shape: any) => shape.id !== action.payload);
        }

        case CLEAR_SHAPE_WHEN_LEAVE_ROOM: {
            return [];
        }

        default:
            return state;
    }
};

export default shapeReducer;
