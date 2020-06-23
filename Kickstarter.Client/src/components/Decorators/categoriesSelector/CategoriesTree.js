import React, {Component} from 'react';
import {Tree} from 'antd';
import PropTypes from 'prop-types';
import { ApiFetch } from '../../../Helpers/ApiCall';
import { CategoryEndpoint } from '../../../Helpers/AppRoutes';
const { TreeNode } = Tree;


export class CategoriesTree extends Component{
    splitSymbol = '-';
    allCategoryKey = 'all';

    constructor(props) {
        super(props);

        this.state = {
            categories: [],
            expandedKeys: props.expandedKey,
            defaultSelectedKey: this.getKey({
                categoryId: props.expandedKey,
                subCategoryId: props.defaultSelectedSubKey
            }),
        };
        
        this.onSelect = this.onSelect.bind(this);
    }
    
    getKey = data => {
        if (data.categoryId === undefined){
            return this.allCategoryKey;
        }
        if (data.subCategoryId === undefined){
            return `${data.categoryId}`;
        }
        
        return `${data.categoryId}${this.splitSymbol}${data.subCategoryId}`
    };

    async componentDidMount() {
        let asd = await ApiFetch(CategoryEndpoint.GetCategories);

        this.setState({
            categories: asd
        });

        console.log(asd)
    }

    onSelect(keys, event){
        if(keys.length === 0){
            return
        }

        this.setState({
            expandedKeys: keys[0],
        });

        let categoryKeys = keys[0] === this.allCategoryKey ? [null, null] : keys[0].split(this.splitSymbol).concat(null);

        let condition = {
            categoryId: categoryKeys[0],
            subCategoryId: categoryKeys[1],
            title: event.node.props.title,
        };

        this.props.handelClick(condition);
    };

    async getSubCategories(id){
        // let data = await ApiRequest.GetSubCategoriesAsync(id);
        // let subCategories = new Array(data.length);
        // for(let i = 0; i < data.length; i++){
        //     subCategories[i] = { title: data[i].subCategoryName, key: this.getKey(data[i]), isLeaf: true};
        // }

        let subCategories = this.state.categories.find(i => i.id === id).subCategories;
        
        return subCategories;
    }

    onLoadSubCategories = async treeNode =>
        new Promise(async resolve => {
            if (treeNode.props.children) {
                resolve();
                return;
            }

            treeNode.props.dataRef.children = await this.getSubCategories(treeNode.props.dataRef.id);

            this.setState({
                categories: [...this.state.categories],
            });
            resolve();
        });

    renderTreeNodes = data =>
        data.map(item => {
            if(item.children){
                return (
                    <TreeNode
                        title={item.categoryName}
                        key={item.id}
                        dataRef={item}
                    >
                        {this.renderTreeNodes(item.children)}
                    </TreeNode>
                )
            }
            return (
                <TreeNode
                    title={item.categoryName}
                    key={item.id}
                    {...item}
                    dataRef={item}
                />
            )
        });

    render() {
        return(
            <Tree
                blockNode={true}
                loadData={this.onLoadSubCategories}
                autoExpandParent={true}
                expandedKeys={[this.state.expandedKeys]}
                defaultSelectedKeys={[this.state.defaultSelectedKey]}
                onSelect={(keys, event) => this.onSelect(keys, event)}>
                <TreeNode title='All' key={this.allCategoryKey} isLeaf={true}/>
                {/* {this.renderTreeNodes(this.state.categories)} */}
            </Tree>
        )
    }
}

CategoriesTree.propTypes = {
    expandedKey: PropTypes.number,
    defaultSelectedSubKey: PropTypes.number,
    handelClick: PropTypes.func.isRequired
};