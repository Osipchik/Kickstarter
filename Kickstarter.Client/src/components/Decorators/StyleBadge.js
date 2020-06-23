import React from "react";
import { withStyles } from '@material-ui/core/styles';
import Badge from '@material-ui/core/Badge';

export const StyledBadge = withStyles(theme => ({
    badge: {
        right: -3,
        top: 9,
        color: theme.palette.green.main
    },
}))(Badge);