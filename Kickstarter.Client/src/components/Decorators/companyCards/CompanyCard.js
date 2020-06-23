import React from 'react';
import Card from "@material-ui/core/Card";
import CardMedia from "@material-ui/core/CardMedia";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import { Progress } from 'antd';
import AccessTimeIcon from '@material-ui/icons/AccessTime';
import {Link} from "react-router-dom";
import missing_project_photo from '../../../assets/missing_project_photo.png';
import {GetDatesDifference} from '../../methods';
import '../../../styles/Card.css'


export const CompanyCard = (props) => {

    const handleClick = (companyId) => props.onClick(companyId);
    
    let categoryPath = `/Explore/${props.value.categoryId}/`;
    // if(props.value.categoryId != null){
    //     categoryPath += props.value.companyCategory.id;
    // }
    
    return(
        <Card className="company-card"
              variant="outlined">
            <CardMedia
                component="img"
                alt={props.value.title}
                image={props.value.imageUrl ?? missing_project_photo}
                title={props.value.title}
                onClick={handleClick}
                className="card-image"
            />
            <CardContent className="pb-1 content-box">
                <Typography component="div"
                            className="card-content"
                            onClick={handleClick}>
                    <Box fontWeight="fontWeightMedium">
                        {props.value.title}
                    </Box>
                    <Box fontSize="fontSize" className="align-self-stretch">
                        {props.value.description}
                    </Box>
                </Typography>
                <div className="align-self-end w-100">
                    <Link to={categoryPath} className="card-link">
                        {props.value.categoryId}
                    </Link>
                    <div className="px-1" onClick={handleClick}>
                        <Typography component="div" className="row">
                            <Box fontWeight="fontWeightBold" fontSize={15}>
                                ${props.value.goal}
                            </Box>
                            <Box fontSize={12} className="text-muted ml-1 flex-grow-1" lineHeight={2}>
                                USD raiced
                            </Box>
                            <Box fontSize={13} textAlign="right" lineHeight={1.9}>
                                {props.value.progress}%
                            </Box>
                        </Typography>
                        <Progress
                            percent={Number(props.value.progress)}
                            showInfo={false}
                            size="small"/>
                        <Typography component="div" className="row">
                            <AccessTimeIcon style={{ fontSize: 15 }}/>
                            <Box fontSize={12}>
                                12 Days left
                            </Box>
                        </Typography>
                    </div>
                </div>
            </CardContent>
        </Card>
    );
}