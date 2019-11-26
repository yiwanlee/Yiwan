import React, { Component } from 'react'
import PropTypes from 'prop-types'
import Comment from './Comment';

class CommentList extends Component {
  static propTypes = {
    comments: PropTypes.array
  }
  static defaultProps = {
    comments: []
  }

  render() {
    return (
      <div>
        {this.props.comments.map((comment, i) => <Comment comment={comment} key={i} />)}
      </div>
    )
  }
}

export default CommentList