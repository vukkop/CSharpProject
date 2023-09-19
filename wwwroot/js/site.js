const BaseUri = "http://localhost:5211";

function getMessageThread(userName, userId) {
  fetch(BaseUri + `/messages/${userName}/thread`)
    .then((response) => response.json())
    .then((data) => {
      renderData(data, userId);
      return data;
    })
    .catch((error) => console.error("Unable to get items.", error));
}

function renderData(dataArray, userId) {
  const container = document.getElementById("threadView");
  container.innerHTML = "";

  dataArray.forEach((m) => {
    const li = document.createElement("li");
    li.innerHTML = `
            <div>
              <span class="float-end">
                  <img class="rounded-circle" style="height: 50px;" src="${absolutePath(
                    m.SenderProfilePhoto
                  )}" alt="User Photo">
                </span>
                <div>
                  <div class="header">
                    <small class="text-muted">
                      <span class="fa fa-clock-o"> ${formatDateTime(
                        m.MessageSent
                      )}</span>
                      ${
                        !m.DateRead && m.SenderId !== userId
                          ? `<span class="text-danger">(unread)</span>`
                          : ""
                      }
                      ${
                        m.DateRead != null && m.SenderId == userId
                          ? `<span class="text-success">(read ${timeAgo(
                              m.DateRead
                            )})</span>`
                          : ""
                      }
                    </small>
                  </div>
                  <p>${m.Content}</p>
                </div>
            </div>
        `;
    container.appendChild(li);
  });
}

function absolutePath(relativePath) {
  return BaseUri + relativePath.replace("~", "");
}

function timeAgo(dateString) {
  const currentDate = new Date();
  const inputDate = new Date(dateString);
  const timeDifferenceInSeconds = Math.floor((currentDate - inputDate) / 1000);

  if (timeDifferenceInSeconds < 60) {
    return "just now";
  } else if (timeDifferenceInSeconds < 3600) {
    const minutes = Math.floor(timeDifferenceInSeconds / 60);
    return `${minutes} ${minutes === 1 ? "minute" : "minutes"} ago`;
  } else if (timeDifferenceInSeconds < 86400) {
    const hours = Math.floor(timeDifferenceInSeconds / 3600);
    return `${hours} ${hours === 1 ? "hour" : "hours"} ago`;
  } else if (timeDifferenceInSeconds < 604800) {
    const days = Math.floor(timeDifferenceInSeconds / 86400);
    return `${days} ${days === 1 ? "day" : "days"} ago`;
  } else {
    const weeks = Math.floor(timeDifferenceInSeconds / 604800);
    return `${weeks} ${weeks === 1 ? "week" : "weeks"} ago`;
  }
}

function formatDateTime(dateTimeString) {
  const messageDate = new Date(dateTimeString);
  const currentDate = new Date();

  if (messageDate.getFullYear() !== currentDate.getFullYear()) {
    const options = {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    };
    return messageDate.toLocaleDateString(undefined, options);
  } else {
    const options = {
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    };
    return messageDate.toLocaleDateString(undefined, options);
  }
}
