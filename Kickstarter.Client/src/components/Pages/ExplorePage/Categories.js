import React from "react";
import Typography from "@material-ui/core/Typography";
import Divider from "@material-ui/core/Divider";
import {CategoriesTree} from "../../Decorators/categoriesSelector/CategoriesTree";
import PropTypes from 'prop-types';

export default function Categories(props) {    
    return(
        <div className="col-md-2 col-lg-3 col-xl-2 d-none d-md-block">
            <Typography
                variant="button"
                display="block"
                gutterBottom
                className="text-center text-muted mt-2">
                category
            </Typography>
            <Divider className="my-3"/>
            <CategoriesTree
                handelClick={props.handelClick}
                defaultSelectedSubKey={props.defaultSelectedSubKey}
                expandedKey={props.expandedKey}
            />
        </div>
    )
}

Categories.propTypes = {
    handelClick: PropTypes.func,
    defaultSelectedSubKey: PropTypes.number,
    expandedKey: PropTypes.number
};