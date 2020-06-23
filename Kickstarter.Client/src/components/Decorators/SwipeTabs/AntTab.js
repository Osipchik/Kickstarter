import React from 'react';
import Tab from '@material-ui/core/Tab';
import { withStyles } from '@material-ui/core/styles';


export const AntTab = withStyles(theme => ({
    root: {
        textTransform: 'none',
        fontWeight: theme.typography.fontWeightRegular,
        '&:hover': {
            color: theme.palette.primary.light,
            opacity: 1,
        },
        '&$selected': {
            color: theme.palette.primary.main,
            fontWeight: theme.typography.fontWeightMedium,
        },
    },
    selected: {},
}))(props => <Tab {...props} />);