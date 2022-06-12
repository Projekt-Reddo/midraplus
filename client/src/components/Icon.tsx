import * as React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { IconDefinition, library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
library.add(fas as unknown as IconDefinition);

const Icon = (props: any) => {
  return <FontAwesomeIcon {...props} fixedWidth={true} />;
};

export default Icon;
