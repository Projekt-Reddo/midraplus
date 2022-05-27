import * as React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
library.add(fas);

const Icon = (props: any) => {
    return <FontAwesomeIcon {...props} fixedWidth={true} />;
};

export default Icon;
