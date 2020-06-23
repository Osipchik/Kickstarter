import React, { useState, useEffect } from "react";
import Divider from "@material-ui/core/Divider";
import TitleCol from "./TitleCol";
import DescriptionCol from "./DescriptionCol";
import MaterialSelect from "../../../Decorators/MaterialSelect";
import {
    updateCategoryId,
    updateDescription, updateProjectImage, updateProjectVideo,
    updateSubCategoryId,
    updateTitle,
    loadBasic
} from "../../../../modules/actions/UpdateBasicPage";
import VideoCol from "./VideoCol";
import PageTitle from '../PageTitle';
import ColSubscribe from '../ColSubscribe';
import { UploadImage } from "../../../Decorators/Uploader/UploadImage";
import { useSnackbar } from "notistack";
import { CustomButton } from "../../../Decorators/Buttons";
import { CreateRequestOptions, CreateRequestImageOptions, GetAuthHeader } from '../../../../Helpers/ApiCall';
import { PreviewEndpoints, CategoryEndpoint } from '../../../../Helpers/AppRoutes';
import { useDispatch, useSelector } from "react-redux";


const {ProjectTitle_Title, ProjectTitleDescription} = {
    ProjectTitle_Title: 'Project title',
    ProjectTitleDescription: 'Write a clear, brief title that helps people quickly understand the gist of your project.'
};

const {CategoryTitle, CategoryDescription} = {
    CategoryTitle: 'Project category',
    CategoryDescription: [
        'Choose the category that most closely aligns with your project.',
        'Think of where backers may look to find it. Reach a more specific community by also choosing a subcategory.',
        'You’ll be able to change the category and subcategory even after your project is live.',
    ]
};

const {ImageTitle, ImageDescription} = {
    ImageTitle: 'Project image',
    ImageDescription: [
        'Add an image that clearly represents your project.',
        'Choose one that looks good at different sizes. It will appear in different sizes in different places: on your project page, across the Kickstarter website and mobile apps, and (when shared) on social channels.\n',
        'You may want to avoid including banners, badges, and text because they may not be legible at smaller sizes.',
        'Your image should be a 4:3 ratio or it will be cropped.'
    ]
};

const {VideoTitle, VideoDescription} = {
    VideoTitle: 'Project video (optional)',
    VideoDescription: [
        'Add a video that describes your project.',
        'Tell people what you’re raising funds to do, how you plan to make it happen, who you are, and why you care about this project.',
        'After you’ve uploaded your video, use our editor to add captions and subtitles so your project is more accessible to everyone.',
    ]
};

const {pageTitle, PageDescription} = {
    pageTitle: 'Start with the basics',
    PageDescription: 'Make it easy for people to learn about your project.'
};

const MainCategoryPlaceholder = 'Select main category';


export const BasicPage = (props) => {
    const company = useSelector(state => state.BasicsPageReducer);
    const dispatch = useDispatch();
    const [categories, setCategories]= useState([]);
    const [subCategories, setSubCategories]= useState([]);
    const [placeholder, setPlaceholder]= useState('No sub categories');
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        const loadPage = async () => {
            let params = new URLSearchParams({
                id: props.companyId,
                auth: true,
            });
            let response = await fetch(PreviewEndpoints.GetPreview + params.toString(),{
                headers: GetAuthHeader(),
                method: 'get'
            });

            if (isResponseOk(response)){
                let result = await response.json();

                if (result.categoryId !== 0) dispatch(updateCategoryId(result.categoryId));
                if (result.subCategoryId !== 0) dispatch(updateSubCategoryId(result.subCategoryId));
                if (result.videoUrl !== null) dispatch(updateProjectVideo(result.videoUrl));
                if (result.imageUrl) dispatch(updateProjectImage(result.imageUrl, null));
                if (result.title !== null) dispatch(updateTitle(result.title));
                if (result.description !== null) dispatch(updateDescription(result.description));
                dispatch(loadBasic(props.companyId));
            }
        }

        const loadCategories = async () => {
            let response = await fetch(CategoryEndpoint.GetCategories,{
                method: 'get'
            });
    
            if (isResponseOk(response)){
                let res = await response.json();
                setCategories(res);
            }
        }

        if (company.id !== props.companyId){
            loadPage();
        }
        loadCategories();
    }, [])

    const isResponseOk = (response, method) => {
        if (response.ok){
            if (method === 'post'){
                showSnackbar('Saved successful', 'success')
            }

            return true;
        }

        if (response.status >= 500) {
            showSnackbar('Unable to load data, server side error. Status: ' + response.status, 'info');
        }
        else if (response.status >= 400) {
            showSnackbar('Client error. Status: ' + response.status, 'warning');
        }
        else if (response.status >= 300) {
            showSnackbar('Redirect missing. Status: ' + response.status, 'warning');
        }

        if (method === 'post'){
            showSnackbar('Unable to save. Status: ' + response.status, 'error');
        }

        return false;
    }

    const showSnackbar = (message, variant) => {
        enqueueSnackbar(message, { variant: variant });
    }

    const selectCategory = (id) => {
        dispatch(updateCategoryId(id));
        let subCategories = categories.find(i => i.id === id).subCategories;
        if(subCategories.length > 0){
            setPlaceholder('Select subcategory (optional)');
            setSubCategories(subCategories);
      
            dispatch(updateSubCategoryId(0));
        }
        else{
            dispatch(updateSubCategoryId(0));

            setPlaceholder('No sub categories');
            setSubCategories([]);
        }
        //This category does not have any sub
    };

    const onSaveClicked = async () => {
        await saveData();
        await saveImage();
    }

    const saveData = async () => {
        let data = {
            Id: props.companyId,
            Title: company.title,
            Description: company.description,
            VideoUrl: company.videoUrl,
            CategoryId: company.categoryId,
            SubCategoryId: company.subCategoryId
        }

        let options = CreateRequestOptions(data, 'post', true);
        let response = await fetch(PreviewEndpoints.UpdatePreviewInfo, options)
        isResponseOk(response, 'post');
    }

    const saveImage = async () => {
        if (company.previewImage.file !== null){
            let imageOptions = CreateRequestImageOptions(company.previewImage.file, 'post');
            let response = await fetch(PreviewEndpoints.UpdatePreviewImage + this.companyId, imageOptions)
            isResponseOk(response, 'post');
        }
    }

    return (
        <div className={props.className}>
            <PageTitle title={pageTitle} subtitle={PageDescription}/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe title={ProjectTitle_Title} description={[ProjectTitleDescription]}/>
                <div className="col mt-09">
                    <TitleCol maxLen={60} onChange={(title) => dispatch(updateTitle(title))} title={company.title}/>
                    <DescriptionCol maxLen={135} onChange={(description) => dispatch(updateDescription(description))} description={company.description}/>
                </div>
            </div>
            <Divider className="mt-4"/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe title={CategoryTitle} description={CategoryDescription}/>
                <div className="col ml-auto">
                    <MaterialSelect
                        className="w-100 my-2"
                        onChange={(categoryId) => selectCategory(categoryId)}
                        placeholder={MainCategoryPlaceholder}
                        items={categories}
                        value={company.categoryId}/>
                    <MaterialSelect
                        className="w-100 mt-1"
                        onChange={(subId) => dispatch(updateSubCategoryId(subId))}
                        placeholder={placeholder}
                        items={subCategories}
                        value={company.subCategoryId}/>
                </div>
            </div>
            <div className="col">
                <Divider className="mt-4"/>
            </div>
            <br/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe title={ImageTitle} description={ImageDescription}/>
                <UploadImage
                    className="col ml-auto mt-3 pt-1"
                    onChange={(url, file) => dispatch(updateProjectImage(url, file))}
                    maxSize={2}
                    cropped={true}
                    previewImageUrl={company.previewImage.url}/>
            </div>
            <Divider className="mt-4"/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe title={VideoTitle} description={VideoDescription}/>
                <VideoCol
                    onChange={(url) => dispatch(updateProjectVideo(url))}
                    videoUrl={company.videoUrl}
                    className="col ml-auto mt1"
                />
            </div>
            <Divider className="mt-4"/>
            <div className="my-4 row">
                <CustomButton variant="contained" size="medium" color="primary"
                              className="mx-auto" onClick={() => onSaveClicked()}>
                     Save
                </CustomButton>
            </div>
        </div>
    )
}