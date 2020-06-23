import React from "react";
import SwipeableViews from "react-swipeable-views";
import PropTypes from 'prop-types';
import { useTheme } from "@material-ui/core/styles";
import { TabPanel } from "./TabPanel";
import { AntTab } from "./AntTab";

function a11yProps(index) {
    return {
        id: `nav-tab-${index}`,
        'aria-controls': `nav-tabpanel-${index}`,
    };
}

export const LinkTab = (props) => {
    return (
        <AntTab
            onClick={event => { event.preventDefault(); }}
            label={props.label}
            href={props.href}
            {...a11yProps(props.index)}
            {...props}
        />
    );
}

LinkTab.propTypes = {
    href: PropTypes.string,
    index: PropTypes.number,
};

export const SwipeTabs = (props) => {
    const theme = useTheme();

    return(
        <SwipeableViews
            axis={theme.direction === 'rtl' ? 'x-reverse' : 'x'}
            index={props.selectedPage}
            className="px0"
            onChangeIndex={props.onChangePage}>
            {props.pages.map((page, idx) => (
                <TabPanel key={idx} value={props.selectedPage} index={idx} className="px0">
                    {page}
                </TabPanel>
            ))}
        </SwipeableViews>
    )
}

SwipeTabs.propTypes = {
    selectedPage: PropTypes.number.isRequired,
    onChangePage: PropTypes.func.isRequired,
    pages: PropTypes.array.isRequired,
};