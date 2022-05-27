import * as React from "react";

// Components
import Setting from "components/Setting";
import Avatar from "components/Avatar";
import UserGuide from "components/UserGuide";

interface HelpPageProps {}

const HelpPage: React.FC<HelpPageProps> = () => {
    return (
        <div
            className="h-auto w-screen text-white"
            style={{ backgroundColor: "var(--bg)" }}
        >
            <UserGuide />
            <Setting />
            <Avatar />
        </div>
    );
};

export default HelpPage;
