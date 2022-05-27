import { CLEAR_SHAPE_WHEN_LEAVE_ROOM, DRAW_SHAPE, REDO, UNDO } from "store/actions";

const shapes: {
    undoStack: object[];
    redoStack: object[];
} = {
    undoStack: [],
    redoStack: [],
};

const shapeReducer = (state = shapes, action: ActionType) => {
    switch (action.type) {
        case DRAW_SHAPE:
            return {
                redoStack: [],
                undoStack: [...state.undoStack, action.payload],
            };

        case UNDO: {
            if (state.undoStack.length === 0) {
                return state;
            }

            const lastUndoShape = state.undoStack.pop();

            return {
                ...state,
                redoStack: [...state.redoStack, lastUndoShape],
            };
        }

        case REDO: {
            if (state.redoStack.length === 0) {
                return state;
            }

            const lastRedoShape = state.redoStack.pop();

            return {
                ...state,
                undoStack: [...state.undoStack, lastRedoShape],
            };
        }

        case CLEAR_SHAPE_WHEN_LEAVE_ROOM: {
            return {
                undoStack: [],
                redoStack: [],
            };
        }

        default:
            return state;
    }
};

export default shapeReducer;
