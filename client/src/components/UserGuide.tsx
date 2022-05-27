// Libs
import * as React from "react";

// Style
import "styles/UserGuide.css";

interface UserGuideProps {}

const mondais = [
    { id: 1, mondai: "What can I do with Dảkboard?" },
    { id: 2, mondai: "How do I start using Dảkboard on Draplus Web?" },
    { id: 3, mondai: "How do I invite other users to my DăkBoard?" },
    { id: 4, mondai: "How do I interact with Dảkboard?" },
    { id: 5, mondai: "How do I change brush width and brush color?" },
    { id: 6, mondai: "Why can't I see the Clear All button?" },
    { id: 7, mondai: "How do I create a new board?" },
    {
        id: 8,
        mondai: "How can I export a DảkBoard, or save it as a screenshot or a file?",
    },
    { id: 9, mondai: "How do I chat with other users on my DảkBoard" },
    { id: 10, mondai: "How do I create straight lines?" },
    { id: 11, mondai: "How can I donate for Draplus developer?" },
    {
        id: 12,
        mondai: "I have got a suggestion or I have found a bug. What should I do?",
    },
];

const kotaes = [
    {
        id: 1,
        mondai: "What can I do with Dảkboard?",
        kotae: [
            "Draplus is a collaborating drawing software based on web application. System support user for many features such as:",
            "- Collaborating drawing",
            "- Quick take note",
            "- Group chat",
            "- View online people",
            "- Brainstorming",
            "- Project management",
            "- Problem solving",
        ],
    },
    {
        id: 2,
        mondai: "How do I start using Dảkboard on Draplus Web?",
        kotae: [
            "To use Dảkboard, you need to sign in on the Draplus Website with a Google Account.",
            "You'll be redirected to the Board List Page, create a board then click on it and you can use Dảkboard.",
        ],
    },
    {
        id: 3,
        mondai: "How do I invite other users to my DăkBoard?",
        kotae: [
            "Dảkboard is an online board and group chat, if you want to invite your friends to a board you can copy the URL of your board or click 'Share' in the settings then share it with your friends your. They can access your board if they've already signed up to the Draplus Website.",
        ],
    },
    {
        id: 4,
        mondai: "How do I interact with Dảkboard?",
        kotae: [
            "Dảkboard has almost the same function as ordinary boards such as paint:",
            "- Brush: draw.",
            "- Eraser: erase.",
            "- Text: write text.",
            "- Note: take note.",
            "- Undo and Redo.",
            "- Export shapes were drawn to PNG image.",
            "You can click each icon to select the tool you want to use.",
        ],
    },
    {
        id: 5,
        mondai: "How do I change brush width and brush color?",
        kotae: [
            "To change brush width, you double click to brush icon, an option board will appear and you can select your expected brush width.",
            "The same with brush color.",
        ],
    },
    {
        id: 6,
        mondai: "Why can't I see the Clear All button?",
        kotae: [
            "Clear All button is special function can only see by room master.",
            "If room master click Clear All button he/she will delete all shape and can't undo this action.",
        ],
    },
    {
        id: 7,
        mondai: "How do I create a new board?",
        kotae: [
            "First you need to register Draplus web, login and click create table button",
            "You can change the title of the board by clicking the three dots on the board",
        ],
    },
    {
        id: 8,
        mondai: "How can I export a DảkBoard, or save it as a screenshot or a file?",
        kotae: [
            "To export your DảkBoard, tap the export icon on the left toolbar.",
        ],
    },
    {
        id: 9,
        mondai: "How do I chat with other users on my DảkBoard",
        kotae: [
            "Click the chat icon at the bottom right when you are in the board",
            "Then text like you do with all other chat apps",
        ],
    },
    {
        id: 10,
        mondai: "How do I create straight lines?",
        kotae: [
            "In this version we don't support it yet so donate and wait we'll do it in a few months",
        ],
    },
    {
        id: 11,
        mondai: "How can I donate for Draplus developer?",
        kotae: [
            "Thank you if you want to support our team, you can contact us via this email:",
            "draplus.info@gmail.com",
        ],
    },
    {
        id: 12,
        mondai: "I have got a suggestion or I have found a bug. What should I do?",
        kotae: [
            "You can question our team in this Github account: ",
            "https://github.com/Projekt-Reddo",
        ],
    },
];

const UserGuide: React.FC<UserGuideProps> = () => {
    const [showDropDown, setShowDropDown] = React.useState(false);

    return (
        <div className="flex pt-28">
            {/* SideBar */}
            <div className="sideBar w-80">
                <div className="scrollContent grid grid-cols-1 gap-2 fixed ml-4 w-[18rem] max-w-[18rem]">
                    {mondais.map((mondai) => (
                        <a
                            href={`#${mondai.id}`}
                            key={mondai.id}
                            className="tabSelect self-center p-2"
                        >
                            {mondai.mondai}
                        </a>
                    ))}
                </div>
            </div>

            {/* Content */}
            <div className="scrollContent mx-4 w-[50rem]">
                {/* DropDown */}
                <div>
                    <div
                        className="dropDown hidden text-white p-2 mb-1"
                        onClick={() => setShowDropDown(!showDropDown)}
                    >
                        On this page
                    </div>

                    <div
                        className={
                            showDropDown
                                ? `frame z-10 list-none mb-2`
                                : `hidden`
                        }
                    >
                        <ul className="p-2">
                            {mondais.map((mondai) => (
                                <li key={mondai.id} className="text-white py-1">
                                    <a
                                        href={`#${mondai.id}`}
                                        onClick={() => setShowDropDown(false)}
                                    >
                                        {mondai.mondai}
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>

                {/* Header */}
                <div>
                    <div className="text-5xl">Draplus DảkBoard Help</div>
                    <div className="mt-6">DảkBoard</div>
                </div>
                {/* Body */}
                <div className="mt-12 mb-10">
                    {/* Introduce */}
                    <div>
                        Online group communication when discussing projects is
                        always a pain when sharing ideas among group members. We
                        design a solution for this problem by providing online
                        collaborating drawing and group chat included.
                    </div>
                    {/* Helps */}
                    {kotaes.map((kotae) => (
                        <div id={`${kotae.id}`} key={kotae.id} className="mt-8">
                            <div className="text-3xl">{kotae.mondai}</div>
                            {kotae.kotae.map((ko) => (
                                <div>{ko}</div>
                            ))}
                        </div>
                    ))}

                    <div className="text-2xl my-4">
                        <div>
                            We appreciate with all your support to this project
                            and our team.
                        </div>
                        <div>Thank you.</div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default UserGuide;
