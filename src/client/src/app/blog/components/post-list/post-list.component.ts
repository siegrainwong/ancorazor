import { Component, OnInit } from '@angular/core';
import { ArticleParameters } from '../../models/article-parameters';
import { PostService } from '../../services/post.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  postParameter = new ArticleParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0 });

  constructor(private postService: PostService) { }

  ngOnInit() {
    this.getPost()
  }

  async getPost() {
    let res = await this.postService.getPagedPosts(this.postParameter);

  }
}