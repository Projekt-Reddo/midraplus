import * as React from "react";
import { RootStateOrAny, useSelector } from "react-redux";

import NoteItem from "./NoteItem";

interface NotesProps {}

const Notes: React.FC<NotesProps> = () => {
    const notes = useSelector((state: RootStateOrAny) => state.notes);

    return (
        <>
            {notes.map((note: Note, index: number) => (
                <NoteItem key={index} note={note} />
            ))}
        </>
    );
};

export default Notes;
