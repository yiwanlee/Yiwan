import React, { Component } from 'react'
import PropTypes from 'prop-types'
import ReactDOM from 'react-dom'
import Header from './containers/Header'
import Content from './containers/Content'
import { createStore } from 'redux'
import { Provider } from 'react-redux'
import './index.css'

const themeReducer = (state, action) => {
    if (!state) return {
        themeColor: '#0f0'
    }
    switch (action.type) {
        case 'CHANGE_COLOR':
            return { ...state, themeColor: action.themeColor }
        default:
            return state
    }
}


const store = createStore(themeReducer)
// 删除 Index 里面所有关于 context 的代码
class Index extends Component {
    render() {
        return (
            <div>
                <Header />
                <Content />
            </div>
        )
    }
}

// 把 Provider 作为组件树的根节点
ReactDOM.render(
    <Provider store={store}>
        <Index />
    </Provider>,
    document.getElementById('root')
)