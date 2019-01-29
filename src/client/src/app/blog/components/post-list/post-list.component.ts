import { Component, OnInit } from '@angular/core';
import { ArticleParameters } from '../../models/article-parameters';
import { PostService } from '../../services/post.service';
import ArticleModel from '../../models/article-model';
import { Pagination } from 'src/app/shared/models/response-model';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  parameter = new ArticleParameters({ orderBy: 'id desc', pageSize: 10, pageIndex: 0 })

  articles: ArticleModel[]
  pagiation: Pagination

  constructor(private postService: PostService) { }

  ngOnInit() {
    this.getPost()
  }

  async getPost() {
    let res = await this.postService.getPagedPosts(this.parameter)
    this.articles = res.data
    this.pagiation = res.pagination
    console.log(this.articles)
  }
}