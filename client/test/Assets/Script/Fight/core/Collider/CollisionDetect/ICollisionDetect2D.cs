
using System.Collections.Generic;

public interface ICollisionDetect2D  {

    void InitAllRect(List<FlGameObject> allFlGameObject);
    void AddRect(Rectangle rect);
    void DoCollisionDetect();
    void RemoveRect(Rectangle rect);
}
